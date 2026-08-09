using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Content.Shared._Mono.Grid;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;

namespace Content.Shared._Mono.ShipLoadout;

/// <summary>
/// This handles...
/// </summary>
public sealed class ShipLoadoutSystem : EntitySystem
{
    [Robust.Shared.IoC.Dependency] private readonly EntityWhitelistSystem _whitelist = default!;
    [Robust.Shared.IoC.Dependency] private SharedGridModifierSystem _gridMod = default!;
    [Robust.Shared.IoC.Dependency] private PrototypeManager _protoManager = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {

    }

    public void AddLoadout(Entity<ShipLoadoutComponent> ent, string loadoutName, Type component)
    {
        var entities = new HashSet<Entity<IComponent>>();
        _gridMod.GetGridEntities(ent.Owner, entities, component);

        var loadoutData = new LoadoutData
        {
            EntityData = new EntityData[entities.Count],
            LoadoutName = loadoutName,
        };

        var i = 0;
        foreach (var gridEnt in entities)
        {
            if (MetaData(gridEnt).EntityPrototype?.ID is not { } protoId)
                return;

            var entityData = new EntityData()
            {
                Coordinates = Transform(gridEnt).Coordinates,
                Id = protoId,
            };

            loadoutData.EntityData[i] = entityData;

            i++;
        }

        ent.Comp.LoadoutData.Add(loadoutData);
    }

    /// <summary>
    /// If loadout data found happens to be invalid - picks first one as a fallback.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="component"></param>
    /// <param name="loadoutName"></param>
    /// <param name="loadoutData"></param>
    /// <returns></returns>
    public bool TryGetLoadoutWithFallback(EntityUid uid, ShipLoadoutComponent? component, string loadoutName, [NotNullWhen(true)] out LoadoutData? loadoutData)
    {
        loadoutData = null;
        if (!Resolve(uid, ref component) ||
            component.LoadoutData.Count == 0)
            return false;
        if (component.LoadoutData.Find(lD => lD.LoadoutName.Equals(loadoutName)) is not { } gridLoadoutData)
            loadoutData = component.LoadoutData[0];
        else
            loadoutData = gridLoadoutData;

        return true;
    }

    public bool TryGetLoadout(EntityUid uid, ShipLoadoutComponent? component, string loadoutName, [NotNullWhen(true)] out LoadoutData? loadoutData)
    {
        loadoutData = null;
        if (!Resolve(uid, ref component))
            return false;
        if (component.LoadoutData.Find(lD => lD.LoadoutName.Equals(loadoutName)) is not { } gridLoadoutData)
            return false;

        loadoutData = gridLoadoutData;
        return true;
    }

    public bool TryGetLoadoutEntity(LoadoutData loadoutData, int index, [NotNullWhen(true)] out EntityData? data)
    {
        data = null;
        if (loadoutData.EntityData.Length < index)
            return false;

        if (loadoutData.EntityData[index] is not { } entData)
            return false;

        data = entData;
        return true;
    }

    public void LoadVoucherLoadout(Entity<ShipLoadoutComponent> gridEnt, Dictionary<string, int> voucherLoadoutData)
    {
        var xform = Transform(gridEnt);

        foreach (var voucherData in voucherLoadoutData)
        {
            if (!TryGetLoadoutWithFallback(gridEnt.Owner, gridEnt.Comp, voucherData.Key, out var loadoutData))
                continue;

            if (!TryGetLoadoutEntity(loadoutData, voucherData.Value, out var loadoutEntity))
                continue;

            var spawnCoords = xform.Coordinates + loadoutEntity.Coordinates;

            var uid = PredictedSpawnAtPosition(loadoutEntity.Id, spawnCoords);
            var loadoutComp = EnsureComp<LoadoutEntityComponent>(uid);

            loadoutComp.LoadoutName = voucherData.Key;
            loadoutComp.LoadoutIndex = voucherData.Value;
        }
    }

    public void SaveVoucherLoadout(Entity<ShipLoadoutComponent> gridEnt, EntityUid voucher)
    {

    }
}
