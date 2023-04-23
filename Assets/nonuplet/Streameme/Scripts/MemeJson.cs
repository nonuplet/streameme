namespace Streameme
{
    /**
 * JINS MEMEの20Hz(current)データ
 */
    [System.Serializable]
    public class MemeCurrentData
    {
        public int blinkSpeed;
        public int blinkStrength;
        public int eyeMoveUp;
        public int eyeMoveDown;
        public int eyeMoveLeft;
        public int eyeMoveRight;
        public float roll;
        public float pitch;
        public float yaw;
        public float accX;
        public float accY;
        public float accZ;
        public bool walking;
        public bool noiseStatus;
        public int fitError;
        public int powerLeft;
        public int sequenceNumber;
    }
}