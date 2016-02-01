using UnityEngine;
using System.Collections;

public interface ITriggerable 
{
	void Trigger();

}

public interface ISwitchable
{
	void ActivateSwitch();
}

public interface IPickup
{
    int ItemNumber { get; }
}