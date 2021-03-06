﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using mzxrules.Helper;
using mzxrules.OcaLib.Actor;

namespace mzxrules.OcaLib.SceneRoom.Commands
{
    class TransitionActorListCommand : SceneCommand, IActorList, IDataCommand
    {
        Game Game { get; set; }
        public SegmentAddress SegmentAddress { get; set; }

        public List<TransitionActorSpawn> TransitionActorList = new List<TransitionActorSpawn>();
        public int TransitionActors { get; set; }

        private delegate TransitionActorSpawn GetTransitionActorRecord(byte[] data);
        readonly GetTransitionActorRecord NewTransitionActor;

        public TransitionActorListCommand(Game game)
        {
            Game = game;

            if (game == Game.OcarinaOfTime)
            {
                NewTransitionActor = ActorSpawnFactory.OcarinaTransitionActors;
            }
            else if (game == Game.MajorasMask)
            {
                NewTransitionActor = ActorSpawnFactory.MaskTransitionActors;
            }
        }

        public override void SetCommand(SceneWord command)
        {
            base.SetCommand(command);
            TransitionActors =  Command.Data1;
            SegmentAddress = Command.Data2;
            if (command[4] != (byte)ORom.Bank.scene)
                throw new Exception();
        }

        public void Initialize(BinaryReader br)
        {
            byte[] actorArray;

            actorArray = new byte[ActorSpawn.SIZE];

            br.BaseStream.Position = SegmentAddress.Offset;
            
            var readRemaining = br.BaseStream.Length - br.BaseStream.Position;
            var maxLoops = readRemaining / ActorSpawn.SIZE;

            var loop = (maxLoops > TransitionActors) ? TransitionActors : maxLoops;
            
            for (int i = 0; i < loop; i++)
            {
                br.Read(actorArray, 0, 16);
                TransitionActorList.Add(NewTransitionActor(actorArray));
            }
        }

        public override string Read()
        {
            string result;
            result = ToString();
            foreach (TransitionActorSpawn a in TransitionActorList)
            {
                result += Environment.NewLine + a.Print();
            }
            return result;
        }
        public override string ToString()
        {
            return $"There are {TransitionActorList.Count} transition actor(s). List starts at {SegmentAddress.Offset:X8}.";
        }

        public List<ActorSpawn> GetActors()
        {
            return TransitionActorList.Cast<ActorSpawn>().ToList();
        }
    }
}