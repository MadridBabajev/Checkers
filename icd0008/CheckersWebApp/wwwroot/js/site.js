// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Tried to have a custom popup windows using bootstrap modal class
// $(() => {
//    
//     const PlaceHolderElement = $('#PlaceHolderHere');
//     $('button[data-toggle="ajax-modal"]').click(function (event) {
//         const url = $(this).data('url')
//         $.get(url).done((data) => {
//             PlaceHolderElement.html(data)
//             PlaceHolderElement.find('.modal').modal('show')
//         })
//     })
//    
//     PlaceHolderElement.on("click", '[data-delete="modal"]', (event) => {
//         const deleteButton = $(this).parent('.modal').find('form');
//        
//     })
// })