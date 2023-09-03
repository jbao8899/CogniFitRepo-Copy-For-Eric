using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;

namespace CogniFitRepo.Client.HttpServices
{
    public interface IBiometricInformationHttpService
    {
        Task<DataResponse<List<BiometricInformationSnapshotDto>>> GetBiometricInformations();
        Task<DataResponse<List<BiometricInformationSnapshotDto>>> GetCurrentUserBiometricInformations();
        Task<DataResponse<BiometricInformationSnapshotDto>> GetBiometricInformation(long? id);
        Task<DataResponse<BiometricInformationSnapshotDto>> CreateBiometricInformation(BiometricInformationSnapshotDto biometricInformation);
        Task<DataResponse<List<BiometricInformationSnapshotDto>>> CreateBiometricInformations(List<BiometricInformationSnapshotDto> biometricInformations);
        Task<DataResponse<BiometricInformationSnapshotDto>> UpdateBiometricInformation(long? id, BiometricInformationSnapshotDto biometricInformation);
        Task<DataResponse<BiometricInformationSnapshotDto>> DeleteBiometricInformation(long? id);
    }
}
