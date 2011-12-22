using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CustomExtensions.DataAnnotations;

namespace Ebuy
{
    public interface IEntity
    {
        /// <summary>
        /// The entity's persistent (database-generated) identifier
        /// </summary>
        /// <remarks>
        /// This property should be considered internal application logic
        /// and is not for public consumption
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        long Id { get; }

        /// <summary>
        /// The entity's unique (and URL-safe) public identifier
        /// </summary>
        /// <remarks>
        /// This is the identifier that should be exposed via the web, etc.
        /// </remarks>
        string Key { get; }
    }

    public abstract class Entity : IEntity, IEquatable<Entity>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; protected set; }

        [Unique, StringLength(50)]
        public virtual string Key
        {
            get { return _key = _key ?? GenerateKey(); }
            protected set { _key = value; }
        }
        private string _key;


        protected virtual string GenerateKey()
        {
            return KeyGenerator.Generate();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Entity)) return false;
            return Equals((Entity) obj);
        }

        public bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Id == 0 || other.Id == 0)
                return Equals(other._key, _key);

            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (Id == 0)
                    return Key.GetHashCode() * 397;

                return Id.GetHashCode();
            }
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }
    }
}