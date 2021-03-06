using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace dotnettools
{
    public class NPCInfo
    {
        public int NPCIndex;
        public string FileName = string.Empty, Name = string.Empty;
        public int MapIndex;
        public Point Location;
        public ushort Rate = 100;
        public ushort Image;
        public Color Colour;
        public bool TimeVisible = false;
        public byte HourStart = 0;
        public byte MinuteStart = 0;
        public byte HourEnd = 0;
        public byte MinuteEnd = 1;
        public short MinLev = 0;
        public short MaxLev = 0;
        public string DayofWeek = "";
        public string ClassRequired = "";
        public bool Sabuk = false;
        public int FlagNeeded = 0;
        public int Conquest;
        public bool IsDefault, IsRobot;
        // public List<int> CollectQuestIndexes = new List<int>(); // 无用字段?
        // public List<int> FinishQuestIndexes = new List<int>(); // 无用字段?

        public NPCInfo(BinaryReader reader, Manager manager)
        {
            Manager Envir = manager;

            if (Envir.LoadVersion > 33)
            {
                NPCIndex = reader.ReadInt32();
                MapIndex = reader.ReadInt32();

                int count = reader.ReadInt32(); // 0, 因为 CollectQuestIndexes 没地方用到
                // for (int i = 0; i < count; i++)
                //     CollectQuestIndexes.Add(reader.ReadInt32());

                count = reader.ReadInt32(); // 0, 因为 FinishQuestIndexes 没地方用到
                // for (int i = 0; i < count; i++)
                //     FinishQuestIndexes.Add(reader.ReadInt32());
            }

            FileName = reader.ReadString();
            Name = reader.ReadString();

            Location = new Point(reader.ReadInt32(), reader.ReadInt32());

            if (Envir.LoadVersion >= 72)
            {
                Image = reader.ReadUInt16();
            }
            else
            {
                Image = reader.ReadByte();
            }

            Rate = reader.ReadUInt16();

            if (Envir.LoadVersion >= 64)
            {
                TimeVisible = reader.ReadBoolean();
                HourStart = reader.ReadByte();
                MinuteStart = reader.ReadByte();
                HourEnd = reader.ReadByte();
                MinuteEnd = reader.ReadByte();
                MinLev = reader.ReadInt16();
                MaxLev = reader.ReadInt16();
                DayofWeek = reader.ReadString();
                ClassRequired = reader.ReadString();
                if (Envir.LoadVersion >= 66)
                    Conquest = reader.ReadInt32();
                else
                    Sabuk = reader.ReadBoolean();
                FlagNeeded = reader.ReadInt32();
            }
        }

        public void Save()
        {
            var npcInfoModel = new NPCInfoModel()
            {
                Id = NPCIndex,
                MapIndex = MapIndex,
                FileName = FileName,
                Name = Name,
                LocationX = Location.X,
                LocationY = Location.Y,
                Image = Image,
                Rate = Rate,
                TimeVisible = TimeVisible,
                HourStart = HourStart,
                MinuteStart = MinuteStart,
                HourEnd = HourEnd,
                MinuteEnd = MinuteEnd,
                MinLev = MinLev,
                MaxLev = MaxLev,
                DayofWeek = DayofWeek,
                ClassRequired = ClassRequired,
                Conquest = Conquest,
                FlagNeeded = FlagNeeded,
            };
            Manager.DB.Insertable(npcInfoModel).ExecuteCommand();
        }
    }

}