using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GunShootLimit : GunBase
{
    public List<UIGunUpdater> uiGunUpdaters;

    public float maxShoot = 5f;
    public float timeToRecharge = 3f;

    private float _currentShoots;
    private bool _recharging = false;

    private void Awake()
    {
        GetAllUIs();
    }

    protected override IEnumerator ShootCoroutine()
    {
        if (_recharging) yield break;

        while (true)
        {
            if (_currentShoots < maxShoot)
            {
                Shoot();
                _currentShoots++;
                CheckRecharge();
                UpdateUI();
                yield return new WaitForSeconds(timeToRecharge);
            }
            /*
            else
            {
                yield return new WaitForSeconds(timeToRecharge);
                _currentShoots = 0;
            }
            yield return new WaitForSeconds(TimeBetweenShots);
            */
        }
    }

    private void CheckRecharge()
    {
        if (_currentShoots >= maxShoot)
        {
            StopShoot();
            StartRecharge();
        }
    }

    private void StartRecharge()
    {
        _recharging = true;
        StartCoroutine(RechargeCoroutine());
    }

    IEnumerator RechargeCoroutine()
    {
        float time = 0;
        while (time < timeToRecharge)
        {
            time += Time.deltaTime;
            Debug.Log("Recharging: " + time);
            uiGunUpdaters.ForEach(i => i.UpdateValue(time / timeToRecharge));
            yield return new WaitForEndOfFrame();
        }
        _currentShoots = 0;
        _recharging = false;
    }

    private void UpdateUI()
    {
        uiGunUpdaters.ForEach(i => i.UpdateValue(maxShoot, _currentShoots));
    }

    private void GetAllUIs()
    {
        uiGunUpdaters = GameObject.FindObjectsOfType<UIGunUpdater>().ToList();
    }
}

