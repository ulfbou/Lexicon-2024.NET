namespace GarageMVC.ViewModels
{
    public class OverviewPageViewModel
    {
        public uint PlacesRemaining { get; init; }
        public IEnumerable<VehicleOverviewViewModel> VehicleOverviews { get; init; }
        public string? SituationSpecificMessage {  get; init; }
    }
}
