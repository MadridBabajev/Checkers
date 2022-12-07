// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getCurrentGameId() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get('id')
}

function addOnClickListeners() {
    addButtonsOnClickListeners();
    const tiles = document.querySelectorAll('.checkersTile');
    
    tiles.forEach(tile => {
        if (tile.dataset.has_piece === "1") {
            tile.addEventListener("click", selectClickListener.bind(null, tile))
        }
    });
}

function selectClickListener(tile) {
    let [x, y] = tile.dataset.coordinates.split(":");
    selectAPiece(x, y, getCurrentGameId());
}

function selectAPiece(x, y, id) {
    
    $.ajax({
        url: `Play?id=${id}&handler=SelectAPiece&x=${x}&y=${y}`,
        success: (data) => {
            const tiles = document.querySelectorAll('.black-tile')
            
            changeSelectedTileColor(x, y, tiles)
            changePossibleMovesColor(data, tiles, x, y)
        }
    });
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
    
    // The format of tile representation -> 1:2, 2:5...
    const selectedTile = `${selectedX}:${selectedY}`;
    
    // All valid moves received from the backend in the corrects format
    const possibleTileAsStrings = getPossibleTilesStrings(possibleTiles);

    // Cycles through all the black tiles and ignores the currently selected tile
    tiles.forEach(tile => {
        console.log(tile)
        if (tile.dataset.coordinates === selectedTile) {} 
        else {
            if (possibleTileAsStrings.includes(tile.dataset.coordinates)) {
                tile.style.backgroundColor = '#27AE60';
                let clonedTile = tile.cloneNode(true);
                clonedTile.addEventListener("click", selectClickListener.bind(null, clonedTile));
                clonedTile.addEventListener("click", makeAMove.bind(null, selectedTile, tile));
                tile.replaceWith(clonedTile);
            } else {
                tile.style.backgroundColor = '#2980B9';
                let clonedTile = tile.cloneNode(true);
                clonedTile.addEventListener("click", selectClickListener.bind(null, clonedTile))
                tile.replaceWith(clonedTile);

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

function makeAMove(selectedTileStr, tile) {
    
    [xFrom, yFrom] = selectedTileStr.split(":");
    [xTo, yTo] = tile.dataset.coordinates.split(":");
    
    const id = getCurrentGameId()
    
    $.ajax({
        url: `Play?id=${id}&handler=MakeAMove`,
        data: jQuery.param({xfrom: `${xFrom}`, yfrom: `${yFrom}`, xto: `${xTo}`, yto: `${yTo}`}),
        success: () => {
            window.location = `Play?id=${id}`;
        },
        error: (error) => alert("Error: " + error)
    });
}

function addButtonsOnClickListeners() {
    document.getElementById("reverse-move-button")
        .addEventListener("click", goOneMoveBack);
    document.getElementById("restart-game-button")
        .addEventListener("click", restartGame);
    document.getElementById("player-surrender-button")
        .addEventListener("click", playerSurrender);
}

function goOneMoveBack() {
    const id = getCurrentGameId()
    $.ajax({
        url: `Play?id=${id}&handler=ReverseMove`,
        success: () => {
            
            window.location = `Play?id=${id}`;
        },
        error: (error) => alert("Error: " + error)
    })
}

function restartGame() {
    
    let toExecute = confirm("Are you sure you want to restart the game?");
    if (!toExecute) return;
    const id = getCurrentGameId()
    $.ajax({
        url: `Play?id=${id}&handler=RestartGame`,
        success: () => {
            window.location = `Play?id=${id}`;
        },
        error: (error) => alert("Error: " + error)
    })
}

function playerSurrender() {
    let toExecute = confirm("Are you sure you want surrender? The game will be over.");
    if (!toExecute) return;
    const id = getCurrentGameId()
    $.ajax({
        url: `Play?id=${id}&handler=PlayerSurrender`,
        success: () => {
            window.location = `Play?id=${id}`;
        },
        error: (error) => alert("Error: " + error)
    })
}
