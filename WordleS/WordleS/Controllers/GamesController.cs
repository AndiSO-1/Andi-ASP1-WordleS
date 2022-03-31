using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordleS.Data;
using WordleS.Models;

namespace WordleS.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public GamesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            IEnumerable<Game> games = await _context.Game.Include(m => m.Attempt).Where(m => m.User.Id == user.Id).ToListAsync();

            return View(games.Where(m => m.Finished == false));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            List<Word> words = await _context.Word.ToListAsync();
            Random rnd = new();

            // Get a random word for the game
            Word? gameWord = words.ElementAtOrDefault(rnd.Next(0, _context.Word.Count()));

            if (gameWord != null)
            {
                Game game = new()
                {
                    Finished = false,
                    Win = false,
                    MaxAttempt = 6,
                    Word = gameWord,
                    User = await _userManager.GetUserAsync(User),
                };

                _context.Game.Add(
                    game
                );

                await _context.SaveChangesAsync();

                return RedirectToAction("play", new { game.Id });
            }

            // TODO make a message that an error occured (no words on DB)
            return NoContent();
        }

        public async Task<IActionResult> Play(int? id, string? message = null, bool? win = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(m => m.Word)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            IdentityUser user = await _userManager.GetUserAsync(User);

            if (game.User == null)
            {
                return new ForbidResult();
            }
            else
            {
                IEnumerable<Attempt> attempts = _context.Attempt.Where(m => m.Game.Id == game.Id);

                List<AttemptViewModel> checkedAttempts = new();

                char[] wordCharArr = game.Word.Value.ToCharArray();
                foreach (Attempt a in attempts)
                {
                    char[] attemptsCharArr = a.Value.ToCharArray();
                    List<Array> checkedChars = new();
                    for (int i = 0; i < attemptsCharArr.Length; i++)
                    {
                        if (attemptsCharArr[i] == wordCharArr[i])
                        {
                            checkedChars.Add(
                                new string[2]
                                {
                                    attemptsCharArr[i].ToString(),
                                    "right"
                                });
                        }
                        else if (wordCharArr.Contains(attemptsCharArr[i]))
                        {
                            checkedChars.Add(
                                new string[2]
                                {
                                    attemptsCharArr[i].ToString(),
                                    "close"
                                });
                        }
                        else
                        {
                            checkedChars.Add(
                                new string[2]
                                {
                                    attemptsCharArr[i].ToString(),
                                    ""
                                });
                        }
                    }
                    checkedAttempts.Add(
                        new AttemptViewModel
                        {
                            Attempt = a,
                            CheckedChars = checkedChars,
                        });
                }

                ViewData["Message"] = message;
                ViewData["Win"] = win;
                ViewData["Attempts"] = checkedAttempts;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Try(int? id, [Bind("Value")] Attempt attempt)
        {
            if (attempt == null || id == null)
            {
                return NotFound();
            }
            Game game = await _context.Game
                .Include(m => m.Word)
                .FirstAsync(m => m.Id == id);
            Word? word = await _context.Word.FirstOrDefaultAsync(m => m.Value == attempt.Value.ToUpper());

            if (game == null)
            {
                return NotFound();
            }

            // If the word doesn't exist return to the view play with a message
            if (word == null)
            {
                return RedirectToAction("play", (id: game.Id, message: "Ce mot n'est pas dans la liste!"));
            }

            int nbAttempts = _context.Attempt.Count(m => m.Game.Id == game.Id);

            if (nbAttempts < 6)
            {
                _context.Attempt.Add(
                      new()
                      {
                          Value = word.Value,
                          Position = nbAttempts,
                          Game = game,
                      }
                  );

                if (game.Word.Value == word.Value)
                {
                    game.Finished = true;
                    game.Win = true;
                    _context.Update(game);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("play", (id: game.Id, win: true));
                }
                else if (nbAttempts == 5)
                {
                    game.Finished = true;
                    game.Win = false;
                    _context.Update(game);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("play", (id: game.Id, win: false, message: "Vous avez perdu le mot était" + game.Word.Value));
                }
            }
            else
            {
                return RedirectToAction("index");
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("play", new { game.Id });
        }
    }
}
