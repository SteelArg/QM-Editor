using Microsoft.Xna.Framework;

namespace QMEditor.Model;

public class Accessory {

    private Asset _asset;

    public Accessory(Asset asset) {
        _asset = asset;
    }

}

public class AccessoryFactory {

    private Asset _asset;

    public AccessoryFactory(Asset asset) {
        _asset = asset;
    }

    public Accessory Create() {
        return new Accessory(_asset);
    }

}