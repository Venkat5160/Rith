import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'custom-pagination',
    templateUrl: './pagination.html',
    styleUrls: ['./pagination.scss']
})

export class CustomPagination {
    constructor() {
    }
    @Input() page: any
    @Input() pageSize: any
    @Input() totalRecords: any;
    @Output() pageClick = new EventEmitter();
    @Output() pageSizeChange = new EventEmitter();

    changePage(page: number) {
        this.pageClick.emit(page);
    }

    changePageSize(pageSize: number) {
        this.pageSizeChange.emit(pageSize);
    }
}
