
namespace LM.Core.Domain
{
    public class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual Uf Uf { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Cidade) obj);
        }
        
        protected bool Equals(Cidade other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Nome != null ? Nome.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Uf != null ? Uf.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
