using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityShoot : PlayerAbilityBase
{
    //public InputAction shootAction;
    public List<UIGunUpdater> uIGunUpdaters;

    [Header("Armas disponíveis (para teste rápido)")]
    public List<GunBase> gunPrefabs;
    public Transform gunPosition;

    private List<GunBase> _instantiatedGuns = new List<GunBase>();
    private GunBase _currentGun;
    private int _currentGunIndex = 0;

    protected override void Init()
    {
        base.Init();
        CreateAllGuns();
        SwitchWeapon(0);

        inputs.Gameplay.Shoot.performed += ctx => StartShoot();
        inputs.Gameplay.Shoot.canceled += ctx => CancelShoot();
    }

    private void Update()
    {
        for (int i = 0; i < gunPrefabs.Count && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchWeapon(i);
            }
        }
    }

    private void CreateAllGuns()
    {
        foreach (var prefab in gunPrefabs)
        {
            var gun = Instantiate(prefab, gunPosition);
            gun.transform.localPosition = Vector3.zero;
            gun.transform.localEulerAngles = Vector3.zero;
            gun.gameObject.SetActive(false);
            _instantiatedGuns.Add(gun);
        }
    }

    public void SwitchWeapon(int index)
    {
        if (index < 0 || index >= _instantiatedGuns.Count)
        {
            Debug.LogWarning($"[{name}] Índice de arma inválido: {index}");
            return;
        }

        if (_currentGun != null)
        {
            _currentGun.StopShoot();
            _currentGun.gameObject.SetActive(false);
        }

        _currentGunIndex = index;
        _currentGun = _instantiatedGuns[index];
        _currentGun.gameObject.SetActive(true);

        Debug.Log($"Arma trocada para: {_currentGun.name} (índice {index})");
    }

    private void StartShoot()
    {
        if (_currentGun == null) return;

        if (!_currentGun.gameObject.activeInHierarchy)
        {
            Debug.LogWarning($"[{name}] Tentando atirar com arma inativa na hierarquia: {_currentGun.name}");
            _currentGun.gameObject.SetActive(true);
        }

        _currentGun.StartShoot();
    }

    private void CancelShoot()
    {
        if (_currentGun == null) return;
        _currentGun.StopShoot();
    }

    // métodos para o botão de UI (touch)
    public void TouchStartShoot()
    {
        StartShoot();
    }

    public void TouchStopShoot()
    {
        CancelShoot();
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityShoot : PlayerAbilityBase
{
    

    public GunBase gunBase;
    public Transform gunPosition;

    private GunBase _currentGun;

    protected override void Init()
    {
        base.Init();
        CreateGun();
        inputs.Gameplay.Shoot.performed += cts => StartShoot();
        inputs.Gameplay.Shoot.canceled += cts => CancelShoot();
    }

    private void CreateGun()
    {
        _currentGun = Instantiate(gunBase, gunPosition);
        _currentGun.transform.localPosition = _currentGun.transform.localEulerAngles = Vector3.zero;
    }

    private void StartShoot()
    {
        _currentGun.StartShoot();
        Debug.Log("Start Shoot");
        
    }
    private void CancelShoot()
    {
        _currentGun.StopShoot();
        Debug.Log("Cancel Shoot");
        
    }
}
*/