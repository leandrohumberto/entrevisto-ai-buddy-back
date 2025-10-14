
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entrevisto.Domain.Entities
{
    public class Vaga
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        public string Titulo { get; set; }
        public string DescricaoVagaOriginal { get; set; }
        public string RoteiroPrincipal { get; set; }
        public string? RoteiroTecnico { get; set; }
        public string? RoteiroComportamental { get; set; }
        public string? RoteiroTriagem { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UpdatedAt { get; set; }
    }
}
