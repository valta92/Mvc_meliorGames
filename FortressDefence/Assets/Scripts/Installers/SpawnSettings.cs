using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SpawnSettings", menuName = "Installers/SpawnSettings")]
public class SpawnSettings : ScriptableObjectInstaller<SpawnSettings>
{
    public GameManager.Settings gameSettings;
    public FortressPresenter.Settings fortSettings;
    public GameInstaller.PrefabSettings prefabSettings;

    public override void InstallBindings()
    {
        Container.BindInstances(gameSettings);
        Container.BindInstances(fortSettings);
        Container.BindInstances(prefabSettings);
    }
}