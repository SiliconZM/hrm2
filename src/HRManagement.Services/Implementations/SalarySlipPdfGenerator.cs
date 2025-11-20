using HRManagement.Models.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace HRManagement.Services.Implementations
{
    public class SalarySlipPdfGenerator
    {
        public static byte[] Generate(SalarySlipDto slip)
        {
            if (slip == null)
                throw new ArgumentNullException(nameof(slip));

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // Header
                    page.Header().Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeColumn().Text("SALARY SLIP").FontSize(24).Bold();
                            row.RelativeColumn().Text($"Slip #: {slip.SlipNumber}").AlignRight().FontSize(11);
                        });
                        column.Item().PaddingBottom(20);
                    });

                    // Content
                    page.Content().Column(content =>
                    {
                        // Employee Information Section
                        content.Item().Text("Employee Information").FontSize(12).Bold();
                        content.Item().PaddingBottom(5);

                        content.Item().Text($"Name: {slip.EmployeeName}").FontSize(10);
                        content.Item().Text($"Period: {slip.Period}").FontSize(10);
                        content.Item().Text($"Status: {slip.Status}").FontSize(10);
                        content.Item().PaddingBottom(15);

                        // Salary Components
                        if (slip.Components != null && slip.Components.Count > 0)
                        {
                            RenderComponentsTable(content, slip.Components);
                            content.Item().PaddingBottom(15);
                        }

                        // Summary Section
                        content.Item().Text("Summary").FontSize(12).Bold();
                        content.Item().PaddingBottom(5);

                        content.Item().Row(row =>
                        {
                            row.RelativeColumn().Text("Gross Salary").Bold();
                            row.RelativeColumn().Text(slip.GrossSalary.ToString("C2")).AlignRight().Bold();
                        });

                        content.Item().Row(row =>
                        {
                            row.RelativeColumn().Text("Total Deductions");
                            row.RelativeColumn().Text(slip.TotalDeductions.ToString("C2")).AlignRight();
                        });

                        content.Item().Row(row =>
                        {
                            row.RelativeColumn().Text("Income Tax");
                            row.RelativeColumn().Text(slip.IncomeTax.ToString("C2")).AlignRight();
                        });

                        content.Item().PaddingBottom(5);

                        content.Item().Row(row =>
                        {
                            row.RelativeColumn().Text("Net Payable").Bold().FontSize(12);
                            row.RelativeColumn().Text(slip.NetPayable.ToString("C2")).Bold().FontSize(12).AlignRight();
                        });
                    });

                    // Footer
                    page.Footer().Column(footer =>
                    {
                        footer.Item().AlignCenter();
                        footer.Item().Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}").FontSize(8).FontColor("999999");

                        if (slip.SalaryCreditedDate.HasValue)
                        {
                            footer.Item().Text($"Salary Credited on {slip.SalaryCreditedDate:yyyy-MM-dd}").FontSize(8).FontColor("999999");
                        }
                    });
                });
            }).GeneratePdf();

            return pdf;
        }

        private static void RenderComponentsTable(ColumnDescriptor content, List<SalarySlipComponentDto> components)
        {
            var earnings = components.Where(c => c.ComponentType == "Earning").OrderBy(c => c.DisplayOrder).ToList();
            var deductions = components.Where(c => c.ComponentType == "Deduction").OrderBy(c => c.DisplayOrder).ToList();

            if (earnings.Count > 0)
            {
                content.Item().Text("Earnings").FontSize(11).Bold();
                content.Item().PaddingBottom(3);

                content.Item().Row(row =>
                {
                    row.RelativeColumn().Text("Component").Bold().FontSize(9);
                    row.RelativeColumn().Text("Amount").Bold().AlignRight().FontSize(9);
                });

                foreach (var component in earnings)
                {
                    content.Item().Row(row =>
                    {
                        row.RelativeColumn().Text(component.ComponentName).FontSize(9);
                        row.RelativeColumn().Text(component.Amount.ToString("C2")).AlignRight().FontSize(9);
                    });
                }

                content.Item().PaddingBottom(10);
            }

            if (deductions.Count > 0)
            {
                content.Item().Text("Deductions").FontSize(11).Bold();
                content.Item().PaddingBottom(3);

                content.Item().Row(row =>
                {
                    row.RelativeColumn().Text("Component").Bold().FontSize(9);
                    row.RelativeColumn().Text("Amount").Bold().AlignRight().FontSize(9);
                });

                foreach (var component in deductions)
                {
                    content.Item().Row(row =>
                    {
                        row.RelativeColumn().Text(component.ComponentName).FontSize(9);
                        row.RelativeColumn().Text(component.Amount.ToString("C2")).AlignRight().FontSize(9);
                    });
                }

                content.Item().PaddingBottom(10);
            }
        }
    }
}
