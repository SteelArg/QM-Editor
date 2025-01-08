namespace QMEditor;

public abstract class Singleton<T> where T : Singleton<T> {

    public static T Instance;

    public Singleton() {
        Instance = (T)this;
    }

    protected abstract void OnSingletonCreated();

}