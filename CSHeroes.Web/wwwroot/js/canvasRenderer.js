const texture = new Image();
texture.src = "assets/textures.png";

const backgrounds = {
  mainMenu: loadImage("assets/Menu.png"),
  camp: loadImage("assets/camp.png"),
  win: loadImage("assets/win.png"),
  defeat: loadImage("assets/defeat.png")
};

function loadImage(src) {
  const image = new Image();
  image.src = src;
  return image;
}

function field(obj, name) {
  const pascal = name[0].toUpperCase() + name.slice(1);
  return obj?.[name] ?? obj?.[pascal];
}

export function render(canvasId, snapshot) {
  const canvas = document.getElementById(canvasId);
  if (!canvas) return;

  const ctx = canvas.getContext("2d");
  ctx.imageSmoothingEnabled = false;
  ctx.clearRect(0, 0, canvas.width, canvas.height);

  const screen = field(snapshot, "screen");
  if (screen === 0 || screen === "MainMenu" || screen === 1 || screen === "NewGame") {
    drawCover(ctx, backgrounds.mainMenu, canvas);
    return;
  }

  if (screen === 3 || screen === "Battle") {
    drawBattle(ctx, field(snapshot, "battle"), canvas);
    return;
  }

  if (screen === 4 || screen === "Camp") {
    drawCover(ctx, backgrounds.camp, canvas);
    return;
  }

  if (screen === 5 || screen === "Win") {
    drawCover(ctx, backgrounds.win, canvas);
    return;
  }

  if (screen === 6 || screen === "Defeat") {
    drawCover(ctx, backgrounds.defeat, canvas);
    return;
  }

  drawExplore(ctx, field(snapshot, "explore"), true);
}

function drawCover(ctx, image, canvas) {
  if (image.complete) {
    ctx.drawImage(image, 0, 0, canvas.width, canvas.height);
  } else {
    image.onload = () => ctx.drawImage(image, 0, 0, canvas.width, canvas.height);
  }
}

function drawExplore(ctx, explore, withGrid) {
  if (!explore) return;

  const background = field(explore, "background") ?? [];
  for (const cell of background) {
    const sprite = field(cell, "sprite");
    drawSprite(ctx, sprite, field(cell, "x") * 32, field(cell, "y") * 32, 32, 32);
  }

  const objects = field(explore, "objects") ?? [];
  for (const object of objects) {
    const sprite = field(object, "sprite");
    drawSprite(ctx, sprite, field(object, "x") * 32, field(object, "y") * 32, 32, 32);
  }

  if (withGrid) drawGrid(ctx, 32, 25, 25);
}

function drawBattle(ctx, battle, canvas) {
  if (!battle) return;

  const background = field(battle, "background");
  for (let y = 0; y < Math.floor(canvas.height / 50); y++) {
    for (let x = 0; x < Math.floor(canvas.width / 50); x++) {
      drawSprite(ctx, background, x * 50, y * 50, 50, 50);
    }
  }

  for (const highlight of field(battle, "highlights") ?? []) {
    const kind = field(highlight, "kind");
    ctx.fillStyle = kind === "enemy" ? "rgba(219,88,86,.35)" : kind === "active" ? "rgba(255,0,0,.35)" : "rgba(70,174,207,.25)";
    ctx.fillRect(field(highlight, "x") * 50, field(highlight, "y") * 50, 50, 50);
  }

  for (const object of field(battle, "objects") ?? []) {
    const x = field(object, "x") * 50;
    const y = field(object, "y") * 50;
    drawSprite(ctx, field(object, "sprite"), x, y, 50, 50);
    const hp = field(object, "hp");
    if (hp !== null && hp !== undefined) {
      ctx.fillStyle = "#16803a";
      ctx.fillRect(x + 24, y + 36, 24, 13);
      ctx.fillStyle = "#fff";
      ctx.font = "10px Segoe UI, sans-serif";
      ctx.textAlign = "center";
      ctx.fillText(String(hp), x + 36, y + 46);
    }
  }

  drawGrid(ctx, 50, Math.floor(canvas.width / 50), Math.floor(canvas.height / 50));
}

function drawSprite(ctx, sprite, dx, dy, dw, dh) {
  if (!sprite || !texture.complete) return;
  ctx.drawImage(
    texture,
    field(sprite, "x"),
    field(sprite, "y"),
    field(sprite, "width"),
    field(sprite, "height"),
    dx,
    dy,
    dw,
    dh
  );
}

function drawGrid(ctx, cell, cols, rows) {
  ctx.strokeStyle = "#000";
  ctx.lineWidth = 1;
  for (let x = 0; x <= cols; x++) {
    ctx.beginPath();
    ctx.moveTo(x * cell + .5, 0);
    ctx.lineTo(x * cell + .5, rows * cell);
    ctx.stroke();
  }
  for (let y = 0; y <= rows; y++) {
    ctx.beginPath();
    ctx.moveTo(0, y * cell + .5);
    ctx.lineTo(cols * cell, y * cell + .5);
    ctx.stroke();
  }
}

export function saveGame(name, payload) {
  localStorage.setItem(`csheroes:${name || "autosave"}`, payload);
}

export function loadGame(name) {
  return localStorage.getItem(`csheroes:${name || "autosave"}`);
}

export function toCanvasPoint(canvasId, clientX, clientY) {
  const canvas = document.getElementById(canvasId);
  if (!canvas) return { x: 0, y: 0 };

  const rect = canvas.getBoundingClientRect();
  return {
    x: (clientX - rect.left) * (canvas.width / rect.width),
    y: (clientY - rect.top) * (canvas.height / rect.height)
  };
}
