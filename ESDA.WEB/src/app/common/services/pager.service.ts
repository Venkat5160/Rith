
export class PagerService {
    public globalPageSize: number = Number(localStorage.getItem("PageSizeVal"));

    fromDate: string = GetFromDate();
    todayFormatedDate: string = GetFormatedDate();
    toDate: string = GetFormatedDate();
    accPage: ActivePages;
    activePages: ActivePages[];
    getPager(totalItems: number, currentPage: number = 1, pageSize: number = this.globalPageSize) {
        // calculate total pages        
        var totalPages = Math.ceil(totalItems / pageSize);
        var isActive = true;


        let startPage: number, endPage: number;
        if (totalPages <= 10) {
            // less than 10 total pages so show all
            startPage = 1;
            endPage = totalPages;
        } else {
            // more than 10 total pages so calculate start and end pages
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }

        // calculate start and end item indexes
        var startIndex = (currentPage - 1) * pageSize;
        var endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

        // create an array of pages to ng-repeat in the pager control
        var pages: any = [];
        this.activePages = [];
        for (let i = 1; i <= totalPages; i++) {
            var isVisiable = false;

            if (totalPages <= 10) {
                isVisiable = true;
            }
            else if ((currentPage - 5) <= i && (currentPage + 5) >= i) {
                isVisiable = true;
            }
            else if (currentPage <= 5 && i <= 10) {
                isVisiable = true;
            }



            this.accPage = new ActivePages();
            this.accPage.pageNo = i;
            this.accPage.isActive = isVisiable;
            pages.push(i);
            this.activePages.push(this.accPage);
        }

        // return object with all pager properties required by the view
        return {
            totalItems: totalItems,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages,
            isActive: isActive,
            activePages: this.activePages,
        };
    }
    public YesterdayDate: string = GetYesterdayDate();

}

export class ActivePages {
    pageNo: number;
    isActive: boolean;
}

function GetFormatedDate() {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    ];
    let dateIn = new Date();
    let yyyy = dateIn.getFullYear();
    let mm = dateIn.getMonth(); // getMonth() is zero-based
    let dd = dateIn.getDate();
    let finalDate = dd + "-" + monthNames[mm] + "-" + yyyy;
    return finalDate;
}

function GetFromDate() {
    let dateIn = new Date();
    let yyyy = dateIn.getFullYear();
    let mm = dateIn.getMonth();

    if (mm < 4) {
        yyyy = yyyy - 1;
    }
    var date = "01-Apr-" + yyyy;
    return date;
}

function GetYesterdayDate() {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    ];
    let dateIn = new Date();
    dateIn.setDate(dateIn.getDate() - 1);
    let yyyy = dateIn.getFullYear();
    let mm = dateIn.getMonth(); // getMonth() is zero-based
    let dd = dateIn.getDate();
    let finalDate = dd + "-" + monthNames[mm] + "-" + yyyy;
    return finalDate;
}
