using UnityEngine;

namespace Core.Entity
{
    public interface ISitting
    {
        public bool IsSitting { get; }
        public void SitDown(Transform placePoint);
        public void StandUp(Transform leavePoint);
    }
}
