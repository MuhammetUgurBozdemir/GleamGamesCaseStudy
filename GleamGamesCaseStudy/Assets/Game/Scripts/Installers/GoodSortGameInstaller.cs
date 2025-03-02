using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class GoodSortGameInstaller : MonoInstaller
{
    #region Injection

    [Inject]
    private void Construct()
    {
    }

    #endregion

    public override void InstallBindings()
    {
        //signal install
        //GameSignalsInstaller.Install(Container);

        Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
    }
}