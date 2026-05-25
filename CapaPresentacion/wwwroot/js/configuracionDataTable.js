document.addEventListener('DOMContentLoaded', function () {
    const table = $('#table').DataTable({
        pageLength: 5,
        lengthChange: false,
        dom: 'rt',
        searching: true,
        info: false,
        paging: true,
    });

    $('#search').on('keyup input', function () {
        table.search(this.value).draw();
    });

    table.on('draw', function () {
        const info = table.page.info();
        const startRecord = info.recordsDisplay === 0 ? 0 : info.start + 1;
        $('#info').text(`Mostrando ${startRecord} a ${info.end} de ${info.recordsDisplay} registros`);

        let paginationHtml = '';
        if (info.pages > 1) {
            let prevDisabled = info.page === 0 ? 'opacity-50 cursor-not-allowed pointer-events-none' : 'hover:bg-zinc-100 hover:text-zinc-900';
            paginationHtml += `<button data-action="previous" class="${prevDisabled} flex items-center justify-center rounded-lg border border-zinc-200 bg-white px-3 py-1.5 text-sm font-medium text-zinc-600 transition-colors">Anterior</button>`;

            for (let i = 0; i < info.pages; i++) {
                let activeClass = i === info.page
                    ? 'bg-red-600 text-white border-red-600'
                    : 'text-zinc-600 bg-white border-zinc-200 hover:bg-zinc-100 hover:text-zinc-900';
                paginationHtml += `<button data-page="${i}" class="${activeClass} flex items-center justify-center rounded-lg border px-3 py-1.5 text-sm font-medium transition-colors">${i + 1}</button>`;
            }

            let nextDisabled = info.page === info.pages - 1 ? 'opacity-50 cursor-not-allowed pointer-events-none' : 'hover:bg-zinc-100 hover:text-zinc-900';
            paginationHtml += `<button data-action="next" class="${nextDisabled} flex items-center justify-center rounded-lg border border-zinc-200 bg-white px-3 py-1.5 text-sm font-medium text-zinc-600 transition-colors">Siguiente</button>`;
        }
        $('#pagination-buttons').html(paginationHtml);
    });

    $('#pagination-buttons').on('click', 'button', function () {
        let action = $(this).attr('data-action');
        let page = $(this).attr('data-page');
        if (action) {
            table.page(action).draw('page');
        } else if (page !== undefined) {
            table.page(parseInt(page)).draw('page');
        }
    });

    table.draw();
});