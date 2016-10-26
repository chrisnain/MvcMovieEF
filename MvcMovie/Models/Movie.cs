namespace MvcMovie.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class Movie
	{
		public int Id { get; set; }
		public string Title { get; set; }
		
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime ReleaseDate { get; set; }
		public string Genre { get; set; }
		public decimal Price { get; set; }
	}
}
