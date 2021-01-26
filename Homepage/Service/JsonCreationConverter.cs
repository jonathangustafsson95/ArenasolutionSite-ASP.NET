using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Homepage.Service
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Remove properties to avoid reference-loop
            if (FieldExists("LeagueMemberShips", jObject))
            {
                jObject = RemoveProperty("LeagueMemberShips", "Player", jObject);
            }
            else if(FieldExists("Leagues", jObject))
            {
                jObject = RemoveProperty("Leagues", "LeagueOwner", jObject);
            }
            else if(FieldExists("leagues", jObject))
            {
                jObject = RemoveProperty("leagues", "leagueMembers", jObject);
            }

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
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
        private JObject RemoveProperty(string field, string prop, JObject jObject)
        {
            foreach (JObject inner in jObject[field].Children<JObject>())
            {
                JProperty drop = inner.Property(prop);
                if(drop != null)
                {
                    drop.Remove();
                }
            }
            return jObject;
        }
    }
}
