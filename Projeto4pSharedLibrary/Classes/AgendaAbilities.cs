namespace Projeto4pSharedLibrary.Classes
{
    public class AgendaAbilities
    {
        public long Id { get; set; } // Chave primária
        public long AgendaId { get; set; } // Chave estrangeira para Agendas
        public string Name { get; set; } = string.Empty; // Nome da habilidade
        public string Description { get; set; } = string.Empty; // Descrição da habilidade

        public Agenda Agenda { get; set; } // Relacionamento com Agendas
    }
}