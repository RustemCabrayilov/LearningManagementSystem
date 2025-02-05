using IronOcr;
using LearningManagementSystem.Application.Abstractions.Services.OCR;
using LearningManagementSystem.Infrastructure.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Application.Abstractions.Services.ElasticService;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Infrastructure.Services.OCR
{
    public class OCRService : IOCRService
    {
        public async Task<List<OCRModel>> GetTextFromFileAsync(IFormFile file,Guid documentId)
        {
            var ocr = new IronTesseract();

            // Create a memory stream to hold the uploaded file data
            using (var memoryStream = new MemoryStream())
            {
                // Copy the uploaded file content to the memory stream
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // Create an OCR input from the memory stream (PDF file)
                using (var input = new OcrInput())
                {
                    input.LoadPdf(memoryStream); // Load the PDF from the memory stream

                    // Perform OCR
                    OcrResult result = ocr.Read(input);

                    var headlinesAndContents = new List<OCRModel>();
                    string currentHeadline = string.Empty;
                    StringBuilder currentContent = new StringBuilder();

                    // Process each page
                    foreach (var page in result.Pages)
                    {
                        var pageTextByLine = page.Text.Split('\n');

                        foreach (var line in pageTextByLine)
                        {
                            var trimmedLine = line.Trim();

                            // Check if the line is a headline
                            if (IsHeadline(trimmedLine))
                            {
                                // Add the previous headline and content
                                if (!string.IsNullOrWhiteSpace(currentHeadline))
                                {
                                    headlinesAndContents.Add(new OCRModel(Guid.NewGuid(),documentId,currentHeadline, currentContent.ToString().Trim()));
                                }

                                // Set new headline and reset content
                                currentHeadline = trimmedLine;
                                currentContent.Clear();
                            }
                            else
                            {
                                // Add content under the current headline
                                currentContent.AppendLine(trimmedLine);
                            }
                        }
                    }

                    // Add the last headline and content
                    if (!string.IsNullOrWhiteSpace(currentHeadline))
                    {
                        headlinesAndContents.Add(new OCRModel(Guid.NewGuid(),documentId,currentHeadline, currentContent.ToString().Trim()));
                    }
                    return headlinesAndContents;
                }
            }
        }

        // Method to check if the line matches any predefined headline
        private bool IsHeadline(string line)
        {
            // Match the line to predefined section headings (e.g., Profile, Technical Skills)
            line = line.Trim();

            return line.Equals(CVHeadlines.Profile, StringComparison.OrdinalIgnoreCase) ||
                   line.Equals(CVHeadlines.TechnicalSkills, StringComparison.OrdinalIgnoreCase) ||
                   line.Equals(CVHeadlines.Projects, StringComparison.OrdinalIgnoreCase) ||
                   line.Equals(CVHeadlines.SofSkills, StringComparison.OrdinalIgnoreCase) ||
                   line.Equals(CVHeadlines.Education, StringComparison.OrdinalIgnoreCase) ||
                   line.Equals(CVHeadlines.Courses, StringComparison.OrdinalIgnoreCase) ||
                   line.Equals(CVHeadlines.Languages, StringComparison.OrdinalIgnoreCase) ||
                   line.Equals(CVHeadlines.Papers, StringComparison.OrdinalIgnoreCase);
        }
    }
}
