using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class MedianOfTwoSortedArrays
	{
		public double FindMedianOfTwoSortedArrays(int[] nums1, int[] nums2) {
			if (nums1.Length == 0 && nums2.Length == 0) {
				throw new InvalidOperationException("both lengths cannot be empty");
			}
			if (nums1.Length == 0) {
				return this.FindMedianOfArrayBetweenIndexes(nums2, 0, nums2.Length - 1);
			} if (nums2.Length == 0) {
				return this.FindMedianOfArrayBetweenIndexes(nums1, 0, nums1.Length - 1);
			}
			var nums1LowIndex = 0;
			var nums1HighIndex = nums1.Length - 1;

			var nums2LowIndex = 0;
			var nums2HighIndex = nums2.Length - 1;

			var numItemsRemainingToSort = (nums1HighIndex - nums1LowIndex + 1) + (nums2HighIndex - nums2LowIndex + 1);
			while (numItemsRemainingToSort > 0) {
				if (numItemsRemainingToSort == 1)
				{
					if (nums1HighIndex >= nums1LowIndex)
					{
						return nums1[nums1HighIndex];
					}
					else
					{
						return nums2[nums2HighIndex];
					}
				}
				else if (numItemsRemainingToSort == 2) {
					if (nums1HighIndex >= nums1LowIndex && nums2HighIndex >= nums2LowIndex) {
						return (nums1[nums1HighIndex] + nums2[nums2HighIndex]) / 2.0;
					} if (nums1HighIndex >= nums1LowIndex) {
						return (nums1[nums1LowIndex] + nums1[nums1HighIndex]) / 2.0;
					} else
					{
						return (nums2[nums2LowIndex] + nums2[nums2HighIndex]) / 2.0;
					}
				}

				// if we have more than 2 rows remaining, need to iterate past the smallest and largest number to get a smaller subset
				if (nums1[nums1LowIndex] < nums2[nums2LowIndex]) {
					nums1LowIndex++;
				} else
				{
					nums2LowIndex++;
				}
				if (nums1[nums1HighIndex] > nums2[nums2HighIndex])
				{
					nums1HighIndex--;
				}
				else
				{
					nums2HighIndex--;
				}
				numItemsRemainingToSort -= 2;
			}
			throw new InvalidOperationException("should never get here");
		}

		private double FindMedianOfArrayBetweenIndexes(int[] nums, int lowIndex, int highIndex) {
			var medianIndex = (lowIndex + highIndex) / 2;
			if ((lowIndex + highIndex) % 2 == 0) {
				return (nums[medianIndex] + nums[medianIndex + 1]) / 2.0;
			}
			return nums[medianIndex];
		}
	}
}
