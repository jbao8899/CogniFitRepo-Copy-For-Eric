using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CogniFitRepo.Server.Repositories

{
    public interface IBiometricInformationRepository
    {
        public IEnumerable<BiometricInformationSnapshotDto> GetBiometricInformation();
        public BiometricInformationSnapshotDto GetBiometricInformationByID(long id);
        public List<BiometricInformationSnapshotDto> GetBiometricInformationForUser(string currentUserId);
        public void PutBiometricInformation(long id, [FromBody] BiometricInformationSnapshotDto biometricInformationSnapshotDto);
        public void PostBiometricInformation(BiometricInformationSnapshot biometricInformationSnapshot);
        public void DeleteBiometricInformation(long id);

    }
}
