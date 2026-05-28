function confirmarEliminacion(id, texto) {
    Swal.fire({
        title: '¿Está seguro?',
        text: texto || 'Este registro será eliminado',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#fa2020',
        cancelButtonColor: '#080708',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed && id !== null) {
            document.getElementById('form-delete-' + id).submit();
        }
    });
}