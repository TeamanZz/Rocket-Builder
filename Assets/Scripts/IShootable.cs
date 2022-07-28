using System.Collections;

public interface IShootable
{
    public IEnumerator ShootRepeatedely();
    public void AllowShoot();
    public void ForbidShoot();
}