using UnityEngine;

public abstract class Trade {

public double currentSuccess;

public abstract void evaluate(); //each subclass of trade will have a different way of evaluating its success. But in this parent class, it's a stub.
}
