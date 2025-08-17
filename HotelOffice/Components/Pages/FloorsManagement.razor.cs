using HotelOffice.Business.Interfaces;
using HotelOffice.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelOffice.Components.Pages
{
    public partial class FloorsManagement : ComponentBase
    {
        [Inject]
        private IFloorService FloorService { get; set; } = default!;

        protected List<Floor>? floors;
        protected Floor currentFloor = new();
        protected bool isEditing = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadFloors();
        }

        private async Task LoadFloors()
        {
            floors = await FloorService.GetAllFloorsAsync();
        }

        protected async Task HandleSave()
        {
            if (string.IsNullOrWhiteSpace(currentFloor.Name)) return;

            if (isEditing)
            {
                await FloorService.UpdateFloorAsync(currentFloor);
            }
            else
            {
                await FloorService.AddFloorAsync(currentFloor);
            }

            currentFloor = new();
            isEditing = false;
            await LoadFloors();
            StateHasChanged();
        }

        protected void StartEdit(Floor floorToEdit)
        {
            currentFloor = new Floor
            {
                Id = floorToEdit.Id,
                Name = floorToEdit.Name
            };
            isEditing = true;
        }

        protected void CancelEdit()
        {
            currentFloor = new();
            isEditing = false;
        }

        protected async Task HandleDelete(int id)
        {
            await FloorService.DeleteFloorAsync(id);
            await LoadFloors();
            StateHasChanged();
        }
    }
}