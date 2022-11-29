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
    
    // TODO MAJOR Before adding an event listener, remove the previous one to prevent duplicates
    tiles.forEach(tile => {
        if (tile.dataset.has_piece === "1") {
            
            tile.removeEventListener("click", selectClickListener.bind(null, tile))
            tile.addEventListener("click", selectClickListener.bind(null, tile))
            
        }
    });
}

function selectClickListener(tile) {
    
    let [x, y] = tile.dataset.coordinates.split(":");
    // const urlParams = new URLSearchParams(window.location.search);

    selectAPiece(x, y, getCurrentGameId());
    return "";
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
    const selectedTile = `${selectedX}:${selectedY}`;
    const possibleTileAsStrings = getPossibleTilesStrings(possibleTiles);
    tiles.forEach(tile => {
        if (tile.dataset.coordinates === selectedTile) {
            
        } else {
            if (possibleTileAsStrings.includes(tile.dataset.coordinates)) {
                tile.style.backgroundColor = '#27AE60';
                tile.addEventListener("click", () => makeAMove(selectedTile, tile))
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

function makeAMove(selectedTileStr, tile) {
    
    [xFrom, yFrom] = selectedTileStr.split(":");
    [xTo, yTo] = tile.dataset.coordinates.split(":");
    console.log(selectedTileStr);
    console.log(tile.dataset.coordinates);
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id')
    
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
}

function goOneMoveBack() {
    console.log("Got here");
    
    $.ajax({
        url: `Play?id=${getCurrentGameId()}&handler=ReverseMove`,
        success: () => {
            
            window.location = `Play?id=${getCurrentGameId()}`;
        },
        error: (error) => alert("Error: " + error)
    })
}

function restartGame() {
    
    let toExecute = confirm("Are you sure you want to restart the game?");
    if (!toExecute) return;
    $.ajax({
        url: `Play?id=${getCurrentGameId()}&handler=RestartGame`,
        success: () => {
            window.location = `Play?id=${getCurrentGameId()}`;
        },
        error: (error) => alert("Error: " + error)
    })
}
