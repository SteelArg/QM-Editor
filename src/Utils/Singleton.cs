namespace QMEditor;

public abstract class Singleton<T> where T : Singleton<T> {

    public static T Instance {get => _instance;}

    private static T _instance;

    public Singleton() {
        _instance = (T)this;
    }

}