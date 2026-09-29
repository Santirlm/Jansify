using Jansify_Santiago_Ramirez.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;

namespace Jansify_Santiago_Ramirez.Helpers
{
    public static class InformeService
    {
        public static void GenerarInformePlaylist(Usuario usuario, Playlist playlist, string ruta)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(11));

                    // cabecera
                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Jansify")
                                    .Bold().FontSize(28).FontColor("#FFB3D9");
                                c.Item().Text($"Playlist: {playlist.Nombre}")
                                    .Bold().FontSize(16).FontColor("#FFFFFF");
                                c.Item().PaddingTop(4).Text($"Creada por {usuario.NombreUsuario}")
                                    .FontSize(11).FontColor("#888888");
                            });
                        });
                        col.Item().PaddingTop(8).LineHorizontal(1).LineColor("#FFB3D9");
                        col.Item().PaddingBottom(8);
                    });

                    // contenido
                    page.Content().Column(col =>
                    {
                        col.Item().PaddingBottom(8).Row(row =>
                        {
                            row.RelativeItem().Text($"{playlist.Canciones?.Count ?? 0} canciones")
                                .FontSize(12).FontColor("#888888");
                        });

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(cols =>
                            {
                                cols.ConstantColumn(30);
                                cols.RelativeColumn(3);
                                cols.RelativeColumn(2);
                                cols.RelativeColumn(2);
                                cols.ConstantColumn(50);
                            });

                            // cabecera tabla
                            tabla.Header(header =>
                            {
                                header.Cell().Background("#FFB3D9").Padding(8)
                                    .Text("#").Bold().FontColor("#0D0D0D");
                                header.Cell().Background("#FFB3D9").Padding(8)
                                    .Text("Canción").Bold().FontColor("#0D0D0D");
                                header.Cell().Background("#FFB3D9").Padding(8)
                                    .Text("Artista").Bold().FontColor("#0D0D0D");
                                header.Cell().Background("#FFB3D9").Padding(8)
                                    .Text("Álbum").Bold().FontColor("#0D0D0D");
                                header.Cell().Background("#FFB3D9").Padding(8)
                                    .Text("Dur.").Bold().FontColor("#0D0D0D");
                            });

                            // filas
                            int i = 1;
                            foreach (var c in playlist.Canciones ?? new List<Cancion>())
                            {
                                var bg = i % 2 == 0 ? "#F5F5F5" : "#FFFFFF";
                                tabla.Cell().Background(bg).Padding(8)
                                    .Text(i.ToString()).FontColor("#333333");
                                tabla.Cell().Background(bg).Padding(8)
                                    .Text(c.Titulo).FontColor("#333333");
                                tabla.Cell().Background(bg).Padding(8)
                                    .Text(c.Album?.Artista?.Nombre ?? "-").FontColor("#333333");
                                tabla.Cell().Background(bg).Padding(8)
                                    .Text(c.Album?.Titulo ?? "-").FontColor("#333333");
                                tabla.Cell().Background(bg).Padding(8)
                                    .Text(c.Duracion.ToString(@"m\:ss")).FontColor("#333333");
                                i++;
                            }
                        });
                    });

                    // pie
                    page.Footer().Row(row =>
                    {
                        row.RelativeItem().Text($"Generado el {DateTime.Now:dd/MM/yyyy} por {usuario.NombreUsuario}")
                            .FontSize(9).FontColor("#888888");
                        row.ConstantItem(50).AlignRight().Text(x =>
                        {
                            x.Span("Página ").FontSize(9).FontColor("#888888");
                            x.CurrentPageNumber().FontSize(9).FontColor("#888888");
                        });
                    });
                });
            }).GeneratePdf(ruta);
        }
    }
}