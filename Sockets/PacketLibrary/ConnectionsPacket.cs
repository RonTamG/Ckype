using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketLibrary
{
    public class ConnectionsPacket : PacketStructure
    {
        private ushort PeopleCount;

        public ConnectionsPacket(ushort Packetlength, List<Person> people)
            : base(Packetlength, 1750)
        {
            PeopleCount = (ushort)people.Count;
            WriteUShort((ushort)people.Count, 4);
            this.People = people;                   
        }

        public ConnectionsPacket()
            : base(4, 1750)
        {

        }

        public ConnectionsPacket(byte[] packet)
            : base(packet)
        {
            PeopleCount = ReadUShort(4);
        }


        public List<Person> People
        {
            get
            {
                Person Current;
                List<Person> lst = new List<Person>();
                int PrevPos = 0;
                for (int i = 0; i < PeopleCount; i++)
                {
                    if (i == 0)
                    {
                        PrevPos = 6;
                        Current = new Person(ReadByteArray(8, ReadUShort(6)));
                        lst.Add(Current);
                    }
                    else
                    {
                        PrevPos = PrevPos + 2 + ReadUShort(PrevPos);
                        Current = new Person(ReadByteArray(PrevPos + 2, ReadUShort(PrevPos)));
                        lst.Add(Current);
                    }
                }
                return lst;
            }
            set
            {
                Person Current;
                int PrevPos=0;
                for (int i = 0; i < value.Count; i++)
                {
                    Current = value[i];
                    if (i == 0)
                    {
                        PrevPos = 6;
                        WriteUShort((ushort)Current.Data.Length, 6);
                        WriteByteArray(Current.Data, 8);
                    }
                    else
                    {
                        WriteUShort((ushort)Current.Data.Length, PrevPos + 2 + ReadUShort(PrevPos));
                        PrevPos = PrevPos + 2 + ReadUShort(PrevPos);
                        WriteByteArray(Current.Data, PrevPos + 2);
                    }
                }
            }
        }

        public static ushort PersonListByteLength(List<Person> lst)
        {
            ushort sum = 0;
            for (int i = 0;i < lst.Count; i++)
            {
                sum += (ushort)(lst[i].Data.Length + 2);
            }
            return sum;
        }

    }
}
