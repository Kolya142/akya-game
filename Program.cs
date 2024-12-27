using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
class Program {
    RenderWindow window = null;
    List<Sprite> cards;
    Game gameo = new();
    Vector2i mouse = new();
    int state = 1;
    List<MouseO.Btn> mouseo_res = new();
    private void draw(Shape d, Vector2f v, Color c) {
        d.Position = v;
        d.FillColor = c;
        window.Draw(d);
    }
    private void draw_cards() {
        draw(new CircleShape(2), new Vector2f(0, 0), Color.Red);
        for (int i = 0; i < gameo.rows.Count; i++) {
            int x = i * 100 + 50;
            for (int j = 0; j < gameo.rows[i].Count; j++) {
                int o = gameo.rows[i][j];
                int y = j * 62;
                if (x <= mouse.X && mouse.X <= x + 40 && y <= mouse.Y && mouse.Y <= y + 60) {
                    if (mouseo_res[0].clicked && j == gameo.rows[i].Count-1) {
                        if (gameo.Grab(i)) {
                            state = 2;
                            return;
                        }
                    }
                }
                cards[o].Position = new Vector2f(x, y);
                window.Draw(cards[o]);
            }
        }
    }
    private void move_cards() {
        for (int i = 0; i < gameo.rows.Count; i++) {
            int x = i * 100 + 50;
            if (x <= mouse.X && mouse.X <= x + 40) {
                if (mouseo_res[0].clicked) {
                    if (gameo.Put(i)) {
                        state = 1;
                        return;
                    }
                }
            }
            for (int j = 0; j < gameo.rows[i].Count; j++) {
                int o = gameo.rows[i][j];
                int y = j * 62;
                cards[o].Position = new Vector2f(x, y);
                window.Draw(cards[o]);
            }
        }
        cards[gameo.grabbed.Value].Position = new Vector2f(mouse.X-20, mouse.Y+10);
        window.Draw(cards[gameo.grabbed.Value]);
    }
    private void main() {
        VideoMode videoMode = new VideoMode(800, 600);
        window = new RenderWindow(videoMode, "Akya game");

        cards = new();

        foreach (string name in "N0 N1 N2 N3 N4 N5 N6 N7 N8 N9 SA SB SC".Split(" ")) {
            Texture tex = new("cards/"+name+".png");
            cards.Add(new Sprite(tex));
        }

        window.Closed += (sender, eventArgs) => window.Close();
        View view = window.GetView();
        window.Resized += (sender, eventArgs) => {
            view.Size = new Vector2f(eventArgs.Width, eventArgs.Height);
            view.Center = new Vector2f(eventArgs.Width/2, eventArgs.Height/2);
            window.SetView(view);
        };

        MouseO.MouseO mouseo = new();

        while (window.IsOpen)
        {
            window.DispatchEvents();

            window.Clear(Color.Black);

            mouse = Mouse.GetPosition(window);


            List<bool> btns = new()
            {
                Mouse.IsButtonPressed(Mouse.Button.Left),
                Mouse.IsButtonPressed(Mouse.Button.Middle),
                Mouse.IsButtonPressed(Mouse.Button.Right)
            };
            mouseo_res = mouseo.update(btns);

            if (state == 1) {
                draw_cards();
            }
            else if (state == 2) {
                move_cards();
            }

            for (int i = 0; i < gameo.rows.Count; i++) {
                if (gameo.rows[i].Count == 0) {
                    gameo.rows[i].Add(gameo.random.Next(0, 13));
                }
            }

            draw(new CircleShape(19), new Vector2f(0, window.Size.Y-40), Color.Magenta);

            if (mouseo_res[0].clicked && mouse.X <= 20 && mouse.Y >= window.Size.Y-20) {
                gameo.init();
            }

            window.Display();
        }
    }
    public static void Main() {
        Program program = new();
        program.main();
    }
}