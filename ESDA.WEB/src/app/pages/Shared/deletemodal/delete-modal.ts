import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'ngbd-modal-confirm',
    templateUrl: './delete-modal.html',
})

export class DeleteModalPopupComponent {

    @Input() warnContent: string;
    @Input() header: string;
    constructor(public modal: NgbActiveModal) { }

    confirm() {
        this.modal.close();
    }
}

