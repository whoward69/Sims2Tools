/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF.OBJF
{
    public enum ObjfIndex
    {
        NONE = -1,
        init,
        main,
        load,
        cleanup,
        queueSkipped,
        allowIntersection,
        wallAdjacencyChanged,
        roomChanged,
        dynamicMultiTileUpdate,
        placement,
        pickup,
        userPlacement,
        userPickup,
        levelInforequest,
        servingSurface,
        portal,
        gardening,
        washHands,
        prep,
        cook,
        surface,
        dispose,
        food,
        pickupFromSlot,
        washDish,
        eatingSurface,
        sit,
        stand,
        clean,
        repair,
        uiEvent,
        restock,
        washClothes,
        startLiveMode,
        stopLiveMode,
        linkObjects,
        messageHandler,
        preRoute,
        postRoute,
        goalCheck,
        reactionHandler,
        alongRouteCallback,
        awareness,
        reset,
        lookatTarget,
        walkOver,
        utilityStateChange,
        setModelByType,
        getModelType,
        delete,
        userDelete,
        justMovedIn,
        preventPlaceInSlot,
        globalAwareness,
        objectUpdatedByDesignMode,
        addObjectInfoToInvToken,
        extractObjectInfoFromInvToken
    }
}
