// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// function convertStringToHtml(content) {
//     const board = document.getElementById("gameBoard")
//     const parser = new DOMParser();
//     let newNode = parser.parseFromString(content, "text/xml");
//    
//     board.appendChild(newNode.documentElement);
// }

function addTileOnClickListeners() {
    const tiles = document.querySelectorAll('.checkersTile')
    // TODO, here, event listeners are added, they will have to be removed 
    //  and adjusted based on player's moves
    tiles.forEach(tile => {
        if (tile.dataset.has_piece === "1") {
            tile.addEventListener('click', () => {
                // TODO currently all tiles are valid,
                //  later, they should pass the check and become invalid when piece gets moved
                let [x, y] = tile.dataset.coordinates.split(":");
                const urlParams = new URLSearchParams(window.location.search);
                
                selectAPiece(x, y, urlParams.get('id'));
            })
        }
    });
}

function selectAPiece(x, y, id) {
    
    $.ajax({
        url: `Play?id=${id}&handler=SelectAPiece&x=${x}&y=${y}`,
        success: (data) => {
            const tiles = document.querySelectorAll('.black-tile')
            
            changeSelectedTileColor(x, y, tiles)
            changePossibleMovesColor(data, tiles, x, y)
        },
        error: (error) => alert("Error: " + error)
    })
}

function changeSelectedTileColor(x, y, tiles) {
    const coordinatesString = `${x}:${y}`
    tiles.forEach(tile => {
        if (tile.dataset.coordinates === coordinatesString) {
            tile.style.backgroundColor = '#1E8449';
        } else {
            tile.style.backgroundColor = '#2980B9';
        }
    })
}

function changePossibleMovesColor(possibleTiles, tiles, selectedX, selectedY) {
    const selectedTile = `${selectedX}:${selectedY}`;
    const possibleTileAsStrings = getPossibleTilesStrings(possibleTiles);
    tiles.forEach(tile => {
        if (tile.dataset.coordinates === selectedTile) {
            
        } else {
            if (possibleTileAsStrings.includes(tile.dataset.coordinates)) {
                tile.style.backgroundColor = '#27AE60';
            } else {
                tile.style.backgroundColor = '#2980B9';
            }
        }
    });
}

function getPossibleTilesStrings(possibleTiles) {
    let retList = [];
    possibleTiles.forEach(list => {
        retList.push(`${list[0]}:${list[1]}`);
    });
    return retList;
}