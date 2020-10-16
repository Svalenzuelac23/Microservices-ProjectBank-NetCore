using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MS.AFORO255.History.Model
{
    public class HistoryTransaction
    {
        [BsonId] //ACA LE ESTMAOS UNDICANDO QUE ESTE CAMPO ES EL QUE DEBE DE SER EL ID QUE USE MONGO DB PARA LA TABLA
        public ObjectId Id { get; set; } //CUANDO ES UNA BASE DE DATOS NO RELACIONAL, LE DEBEMO AGREGAR ESTA PROPIEDAD A AL COLECION (TABLA) QUE ES EL ID UNICO
        public int IdTransaction { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string CreationDate { get; set; }
        public int AccountId { get; set; }
    }
}
