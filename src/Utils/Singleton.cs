namespace QMEditor;

public abstract class Singleton<T> where T : Singleton<T> {

    public static T instance;

    public Singleton() {
        instance = (T)this;
    }

    protected abstract void OnSingletonCreated();

}