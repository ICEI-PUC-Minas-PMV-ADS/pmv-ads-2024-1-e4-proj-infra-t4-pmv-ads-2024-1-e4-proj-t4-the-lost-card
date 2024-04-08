using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions.Serialization;

/// <summary>
/// This makes sure the contracts API 
/// </summary>
/// <typeparam name="TBase"></typeparam>
public interface IJsonDerivedTypeBase { }

/// <summary>
/// This makes sure the contracts API 
/// </summary>
/// <typeparam name="TBase"></typeparam>
public interface IJsonDerivedType<TBase> : IDiscriminator, IJsonDerivedTypeBase where TBase : IJsonDerivedTypeBase { }
