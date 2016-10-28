namespace MvcMovie.Controllers
{
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using Microsoft.EntityFrameworkCore;
	using MvcMovie.Data;
	using MvcMovie.Models;

	public class MoviesController : Controller
	{
		private readonly ApplicationDbContext context;

		public MoviesController(ApplicationDbContext context)
		{
			this.context = context;    
		}

		// GET: Movies
		public async Task<IActionResult> Index(string movieGenre, string searchString)
		{
			// Use LINQ to get list of genres.
			var genreQueryable = from m in this.context.Movie
								 orderby m.Genre
								 select m.Genre;

			var movies = from m in this.context.Movie 
						 select m;

			if (!string.IsNullOrEmpty(movieGenre))
			{
				movies = movies.Where(s => s.Genre == movieGenre);
			}

			if (!string.IsNullOrEmpty(searchString))
			{
				movies = movies.Where(s => s.Title.Contains(searchString));
			}

			var movieGenreViewModel = new MovieGenreViewModel
										{
											Genres = new SelectList(await genreQueryable.Distinct().ToListAsync()),
											Movies = await movies.ToListAsync(),
											MovieGenre = movieGenre,
											SearchString = searchString
										};

			return this.View(movieGenreViewModel);
		}

		// GET: Movies/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return this.NotFound();
			}

			var movie = await this.context.Movie.SingleOrDefaultAsync(m => m.Id == id);
			if (movie == null)
			{
				return this.NotFound();
			}

			return this.View(movie);
		}

		// GET: Movies/Create
		public IActionResult Create()
		{
			return this.View();
		}

		// POST: Movies/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Genre,Price,ReleaseDate,Title,Rating")] Movie movie)
		{
			if (this.ModelState.IsValid)
			{
				this.context.Add(movie);
				await this.context.SaveChangesAsync();
				return this.RedirectToAction("Index");
			}

			return this.View(movie);
		}

		// GET: Movies/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return this.NotFound();
			}

			var movie = await this.context.Movie.SingleOrDefaultAsync(m => m.Id == id);
			if (movie == null)
			{
				return this.NotFound();
			}

			return this.View(movie);
		}

		// POST: Movies/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Genre,Price,ReleaseDate,Title,Rating")] Movie movie)
		{
			if (id != movie.Id)
			{
				return this.NotFound();
			}

			if (this.ModelState.IsValid)
			{
				try
				{
					this.context.Update(movie);
					await this.context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!this.MovieExists(movie.Id))
					{
						return this.NotFound();
					}
					else
					{
						throw;
					}
				}

				return this.RedirectToAction("Index");
			}

			return this.View(movie);
		}

		// GET: Movies/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return this.NotFound();
			}

			var movie = await this.context.Movie.SingleOrDefaultAsync(m => m.Id == id);
			if (movie == null)
			{
				return this.NotFound();
			}

			return this.View(movie);
		}

		// POST: Movies/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var movie = await this.context.Movie.SingleOrDefaultAsync(m => m.Id == id);
			this.context.Movie.Remove(movie);
			await this.context.SaveChangesAsync();
			return this.RedirectToAction("Index");
		}

		private bool MovieExists(int id)
		{
			return this.context.Movie.Any(e => e.Id == id);
		}
	}
}
