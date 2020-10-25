﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ModalContainer
    {
        [Inject]
        private ModalService ModalService { get; set; }

        private readonly List<ModalRef> _modalRefs = new List<ModalRef>();

        protected override void OnInitialized()
        {
            ModalService.OnModalOpenEvent += ModalService_OnModalOpenEvent;
            ModalService.OnModalCloseEvent += ModalService_OnModalCloseEvent;
        }

        private async Task ModalService_OnModalOpenEvent(ModalRef modalRef)
        {
            if (!_modalRefs.Contains(modalRef))
            {
                _modalRefs.Add(modalRef);
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task ModalService_OnModalCloseEvent(ModalRef modalRef)
        {
            modalRef.Config.Visible = false;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(250);
            if (modalRef.Config.DestroyOnClose && _modalRefs.Contains(modalRef))
            {
                _modalRefs.Remove(modalRef);
            }
        }


        protected override void Dispose(bool disposing)
        {
            ModalService.OnModalOpenEvent -= ModalService_OnModalOpenEvent;
            ModalService.OnModalCloseEvent -= ModalService_OnModalCloseEvent;

            base.Dispose(disposing);
        }
    }
}