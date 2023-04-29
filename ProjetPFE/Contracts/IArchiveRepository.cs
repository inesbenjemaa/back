using ProjetPFE.Dto;
using ProjetPFE.Entities;

namespace ProjetPFE.Contracts
{
    public interface IArchiveRepository
    {


        public Task<IEnumerable<archive>> GetArchives();
        public Task<archive> GetArchive(int archive_id);
        public Task<archive> CreateArchive(ArchiveForCreationDto archive);
        public Task UpdateArchive(int archive_id, ArchiveForUpdateDto archive);
        public Task DeleteArchive(int archive_id);


    }
}
