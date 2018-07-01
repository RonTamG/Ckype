using System.Net;

namespace Ckype.Core.Networking
{
    /// <summary>
    /// Represents a connected person
    /// </summary>
    public class Person
    {
        #region Public properties
        /// <summary>
        /// The name of this person
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The ip address this person is connected from
        /// </summary>
        public IPAddress Ip { get; set; }
        /// <summary>
        /// The port this person is connected from
        /// </summary>
        public int Port { get; set; }

        #endregion

        #region Constructor
        public Person(string name, IPAddress ip, int port)
        {
            Name = name;
            Ip = ip;
            Port = port;
        } 
        #endregion

        #region Public methods

        /// <summary>
        /// Describes the current person
        /// </summary>
        /// <returns>A string description of the person</returns>
        public override string ToString()
        {
            return $"Name:{Name} Address:{Ip} : {Port}";
        }

        public override int GetHashCode()
        {
            return Ip.GetHashCode() ^ Port.GetHashCode();
        }

        /// <summary>
        /// Checks if two person objects have the same ip and port.
        /// In other words, check if they are equal in value
        /// </summary>
        /// <param name="obj">The person to check against</param>
        /// <returns>True if the ip and port of both objects match, else returns false</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Person))
                return false;

            return this.Ip == ((Person)obj).Ip && this.Port == ((Person)obj).Port;
        }

        public static bool operator==(Person P1, Person P2)
        {
            if (P1 == null || P2 == null)
                return false;

            return P1.Ip == P2.Ip && P1.Port == P2.Port;
        }

        public static bool operator !=(Person P1, Person P2)
        {
            if (P1 == null || P2 == null)
                return true;

            return !(P1.Ip == P2.Ip && P1.Port == P2.Port);
        }

        #endregion
    }
}