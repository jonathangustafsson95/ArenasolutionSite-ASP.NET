using CommonLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homepage.Service
{
    public class UserObjectConverter : JsonCreationConverter<User>
    {
        protected override User Create(Type objectType, JObject jObject)
        {
            // figure out what type of object to construct and populate
            if (FieldExists("balance", jObject))
            {
                return new Advertiser();
            }
            else if (FieldExists("rating", jObject))
            {

                return new Player();
            }
            else if (FieldExists("leagues", jObject))
            {
                return new LeagueOwner();
            }
            else if (FieldExists("admin", jObject))
            {
                return new Operator();
            }
            else
            {
                return new User();
            }
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            string upperFieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);
            if (jObject[fieldName] != null || jObject[upperFieldName] != null)
            {
                return true;
            }
            return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        { }
    }
}
