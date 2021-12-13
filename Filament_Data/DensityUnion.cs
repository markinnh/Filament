using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Filament_Data
{
    public class DensityUnion<T1, T2> : IDensity where T1 : IDensity, new()
        where T2 : IDensity, new()
    {
        private T1 v1;
        public T1 V1
        {
            get => v1;
            set
            {
                v1 = value;
                if (!ReferenceEquals(value, null) && !ReferenceEquals(v2, null))
                    v2 = default(T2);
            }
        }
        private T2 v2;
        public T2 V2
        {
            get => v2;
            set
            {
                v2 = value;
                if (!ReferenceEquals(value, null) && !ReferenceEquals(v1, null))
                    v1 = default(T1);
            }
        }
        [JsonIgnore]
        public IDensity Union => ReferenceEquals(V1, null) ? (IDensity)V2 : (IDensity)V1;
        [JsonIgnore]
        public double Density => Union?.Density ?? default;
        public static implicit operator DensityUnion<T1, T2>(T1 t1)
        {
            return new DensityUnion<T1, T2>() { V1 = t1 };
        }
        public static implicit operator DensityUnion<T1, T2>(T2 t2)
        {
            return new DensityUnion<T1, T2>() { V2 = t2 };
        }
    }
}
