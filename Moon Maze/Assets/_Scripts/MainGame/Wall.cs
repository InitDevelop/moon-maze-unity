public class Wall {

    public int ID;
    public int X;
    public int Z;
    public float Y;
    public int TYPE;
    public bool isActive;

    // TYPE : X type is 0, Z type is 1 (X type means the small face of the wall is heading X direction)

    public Wall(int ID, int X, int Z, int TYPE, bool isActive) {
        this.ID = ID;
        this.X = X;
        this.Z = Z;
        this.TYPE = TYPE;
        this.isActive = isActive;

        if (isActive) {
            Y = 0.5f;
        } else {
            X = 0;
            Z = 0;
            Y = -10f;
        }
    }

    public void deactivate() {
        isActive = false;
        X = 0;
        Z = 0;
        Y = -10f;
    }

    public void activate() {
        isActive = true;
        Y = 0.5f;
    }



}