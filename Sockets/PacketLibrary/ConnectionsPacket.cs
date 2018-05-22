using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//special thanks to LIOR BRUMBERG who massaged me while i was working. Success!

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
                // The current person we are reading from the buffer.
                Person Current;
                // The list of people.
                List<Person> lst = new List<Person>();
                // the index in the buffer where the last person's length of data was.
                int PrevPos = 0;

                for (int i = 0; i < PeopleCount; i++) // Find all people.
                {
                    // if its the first person
                    if (i == 0)
                    {
                        // position of length and position of data are known.
                        PrevPos = 6;
                        // current person is read from the byte array.
                        // first one's length is at 6, and the data starts at 8.
                        Current = new Person(ReadByteArray(8, ReadUShort(6)));
                        lst.Add(Current);
                    }
                    // any other case
                    else
                    {
                        // position is calculated based on the length of the previous data
                        // and the previous position.
                        PrevPos = PrevPos + 2 + ReadUShort(PrevPos);
                        // any other person's length is at PrevPos, which is now the place of current's length
                        // and his data starts 2 bytes later because the length is a ushort.
                        Current = new Person(ReadByteArray(PrevPos + 2, ReadUShort(PrevPos)));
                        lst.Add(Current);
                    }
                }
                // return the full list of people.
                return lst;
            }
            set
            {
                // The current person we will be writing to the list.
                Person Current;
                // the index in the buffer where the last person's length of data was.
                int PrevPos =0;

                for (int i = 0; i < value.Count; i++)
                {
                    // assigns a person to current.
                    Current = value[i];
                    // the first one's place in the array is always the same.
                    if (i == 0)
                    {
                        // everything up to 6 is [length, type, PeopleCount] 2 bytes each.
                        PrevPos = 6;
                        // write his length to the byte array
                        WriteUShort((ushort)Current.Data.Length, 6);
                        // after it write his data to the byte array.
                        WriteByteArray(Current.Data, 8);
                    }
                    // for everyone else the place is calculated based on 
                    // the person who came before them and their length.
                    else
                    {
                        // The persons previous position of data + the length of the data = new length position.
                        WriteUShort((ushort)Current.Data.Length, PrevPos + 2 + ReadUShort(PrevPos));
                        PrevPos = PrevPos + 2 + ReadUShort(PrevPos);
                        // write to array.
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
